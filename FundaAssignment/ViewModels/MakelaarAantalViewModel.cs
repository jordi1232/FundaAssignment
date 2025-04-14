using System.Collections.ObjectModel;
using System.ComponentModel;
using FundaAssignment.Models;
using FundaAssignment.Services;

namespace FundaAssignment.ViewModels
{
    public class MakelaarAantalViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly FundaWebService _service = new();

        private ObservableCollection<MakelaarAantallen> _occurrences = new();

        private bool _canceled = false;
        private bool _isRunning = false;

        /// <summary>
        /// Sorts the total list of occurences in descending order and returns only the top 10
        /// </summary>
        public ObservableCollection<MakelaarAantallen> Top10
        {
            get
            {
                // Future improvement would be to replace this with a more efficient way to keep track of the top 10.
                // As this always reorders the array it is not very efficient. Instead, the AddMakelaar function could check if reordering is needed,
                // and if so only reorder the makelaar that had an occurrence added.
                return [.. _occurrences.OrderByDescending(x => x.Aantal).Take(10)];
            }
        }

        /// <summary>
        /// Used by the progress bar
        /// </summary>
        public int MaxItems
        {
            get
            {
                return _service?.TotalEntries ?? 0;
            }
        }

        /// <summary>
        /// Used by the progress bar
        /// </summary>
        public int CompletedEntries
        {
            get
            {
                return _service?.ReturnedEntries ?? 0;
            }
        }

        /// <summary>
        /// Used to display the progress of the Funda service
        /// </summary>
        public string LoadText
        {
            get
            {
                return $"{CompletedEntries}/{MaxItems}";
            }
        }

        /// <summary>
        /// Used to enable/disable the Start buttons
        /// </summary>
        public bool CanStart
        {
            get
            {
                return !_isRunning;
            }
        }

        /// <summary>
        /// Used to enable/disable the Cancel button
        /// </summary>
        public bool CanCancel
        {
            get
            {
                return _isRunning;
            }
        }

        /// <summary>
        /// Starts the process of sending requests to the Funda API.
        /// Can be interrupted by simply setting the _canceled boolean to true.
        /// </summary>
        /// <param name="searchParams">Array of string used in the for the zo parameter in the query</param>
        public void StartRequesting(string[] searchParams)
        {
            LoggingService.Instance.Clear();
            LoggingService.Instance.Log($"Starting requests with search params: {string.Join(", ", searchParams)}");
            _occurrences.Clear();
            _canceled = false;
            _service.Init(searchParams);

            // Running the loop on a separate thread allows the UI to remain responsive.
            Task.Run(async () =>
            {
                Tuple<bool, string[]> response;
                _isRunning = true;
                TriggerButtonChanges();
                do
                {
                    response = await _service.GetNextMakelaars();
                    foreach (var makelaar in response.Item2)
                    {
                        AddMakelaar(makelaar);
                    }
                    TriggerPropertiesChanged();
                    // To attempt to stick to < 100 requests per minute, we will pause the spam thread for 600ms (60.000ms / 100 requests).
                    // Together with the network delay performing the get should put us below the 100 requests per minute threshold.
                    Thread.Sleep(600);
                } while (response.Item1 && !_canceled);
                // The first item in the tuple is a boolean indicating whether there are more items or not
                // The second is a String[] with the next set of makelaar names
                LoggingService.Instance.Log("DONE");
                _isRunning = false;
                TriggerButtonChanges();
            });
        }

        /// <summary>
        /// Function used by the UI to stop the requests prematurely.
        /// </summary>
        public void Stop()
        {
            LoggingService.Instance.Log("Stopping requests...");
            _canceled = true;
        }

        private void AddMakelaar(string makelaar)
        {
            foreach (var occurrence in _occurrences)
            {
                if (occurrence.MakelaarNaam == makelaar)
                {
                    occurrence.Aantal++;
                    return;
                }
            }

            _occurrences.Add(new MakelaarAantallen(makelaar));
        }

        private void TriggerPropertiesChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Top10)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxItems)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CompletedEntries)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoadText)));
        }

        private void TriggerButtonChanges()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanStart)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanCancel)));
        }
    }
}

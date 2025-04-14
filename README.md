# Funda Assignment

## How to run
Just add a correct key to the URIHelper and run the application. 

## Process
I decided to create this application as a WPF application using the MVVM design pattern. This would allow me to start setting up the application quickly, while also allowing me to have a fairly simple setup for a responsive UI. 

The first step was to see what a response from the API would look like and which data from the response I would be intersted in.
I also used this time to see what the maximum amount of objects could be in the response (as the example URI showed pagination).

Then I worked out a basic web service that would be able to communicate with the API in a continuous way (using pagination).

Finally all that remained was to allow the Observable Collection in the ViewModel to be updated and for the UI to represent the updated data.

## Choices
I chose to use WPF in combincation with the MVVM design pattern as this combination allowed me to have a small setup time in which I could make a responsive UI.

I wanted a responsive UI as the dataset can be fairly large (a few thousand records) and I did not want to have to wait to complete all the requests before returning a result.
This way, the user can see the top 10 develop as the requests are sent out.

Also, to keep the implementation simple I am using the built in .OrderByDescending after each request to retrieve the top 10.
Considering the maximum the API could ever return being about 62k records (in combination with a simple int comparison), this works fine.
However, if the dataset were larger it would be better to have a more optimised algorithm to acquire the top 10.

## Used libraries
- Newtonsoft.Json (13.0.3)

## Other sources
https://github.com/github/gitignore/blob/main/VisualStudio.gitignore for wpf gitignore
# Function_SpotifyDiscoverWeekly 
This is a repo containing an Azure function I created that will back up my Spotify Discover Weekly playlist. 

## Description
This repo contains an Azure function I created that will back up my Spotify Discover Weekly playlist. The function is set on a timer trigger that runs once a week. It is set to run at 2AM UTC every Sunday. Once kicked off the function interacts with the Spotify APIs to get and set data. The function starts off by getting all the playlists listed under my account. After that it finds the IDs for my Discover Weekly and Discover Last Weekly playlist. It then gets all the tracks from my Discover Weekly playlist and puts them in my Discover Last Weekly playlist by replacing what is currently there. By doing all of this I have one more week to go through my music and save off the songs that I enjoyed.

## Install
Installing this application on your local machine should be simple. You need to make sure you have NET Core Version 3.1 installed. Then you can clone the repo in Visual Studio and open the solution file. 

## Prerequisite
Before being able to run this code you'll need to have a few things set up first. First, you'll need a Spotify account. Next, you'll need to allow the app to have access to your account. After you have done that, you'll need to grant the correct authorization scopes. More info found here, [Authorization Scopes](https://developer.spotify.com/documentation/general/guides/authorization/scopes/).

## Use
This project is intended to be hosted and ran in Azure with the supporting infrastructure. You can run it locally for debugging or as a one off if needed. To run in Azure you will need some infrastructure set up with it. You'll need an app service plan and application insight at the very minimum. With it being hosted in Azure it will trigger every time the timer comes up. You won't have to do anything, and your playlists will be backed up automatically. 

## License
[GNU GPLv3](https://choosealicense.com/licenses/gpl-3.0/)

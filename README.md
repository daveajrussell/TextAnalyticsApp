# TextAnalyticsApp

A fairly quickly hacked together project to play around with Azure Text Analytics, by running tweets tweeted @ users to gather the sentiment (i.e. how well perceived is this user on twitter?)

The TweetScraper will run over a list of users (defined in `DataSeed`) and get all tweets tweeted @ them in some arbitrary time frame.

The body is then ran through the Text Ananlytics service and the score is stored alongside the tweet.

The web app is a basic bootstrapped MVC thing (i.e. a mess) that'll plot the data on a highcharts chart.

The backend is probably way overengineered for a demo project, but it's a version (probably slightly more up to date) of a set of core libraries that I've put together for use in side projects that need EF and a nice way of getting at the data.

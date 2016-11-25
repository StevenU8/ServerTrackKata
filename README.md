# ServerTrackKata

Instructions
In this project, two web API endpoints are necessary. They are:
 
Record load for a given server This should take a:
 
1.      server name (string)
a.      CPU load (double)
b.      RAM load (double) And apply the values to an in-memory model used to provide the data in endpoint #2.
2.      Display loads for a given server This should return data (if it has any) for the given server:
a.      A list of the average load values for the last 60 minutes broken down by minute
b.      A list of the average load values for the last 24 hours broken down by hour
 
Assume these endpoints will be under a continuous load being called for thousands of individual servers every minute.
 
There is no need to persist the results to any permanent storage, just keep the data in memory to keep things simple.

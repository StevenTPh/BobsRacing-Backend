# Olympics 100m Sprint Simulator

The 100-meter race is one of the most iconic events in the Summer Olympics. It is known for determining the fastest sprinter in the world. Athletes run a full 100 meters on a straight track, competing for the gold medal. Every year, the world holds its breath as the sprinters take their marks!

The project aims to simulate 100m race sprints with olympic athletes. The athletes can compete against eachother in a customized race that uses SignalR for real-time streaming of data that makes the users see the race happening in real-time, or LIVE. To increase the thrill of the race, the users may also place bets on athletes within a race, with odds calculated based on win probabilty and amount of bets placed on athlete.
The application features CRUD operations on Race, Athletes, bets and the user. The frontend is deployed to azure, and can be accessed and used if the user has a local backend. The project utilizes MySQL in Azure for a robust database solution that gives the responsibility of maintenance towards Azure. 



## Technology

- Frontend: The application is built with vite, using react and typescript as the code language.
- Backend: The application is built with .NET 6 and ASP.NET core as the code language.
- JWT Token is used to secure the application. The user has no access to the application unless signed in. This prevents the user to perform malicious actions towards the backend from the application. 
- SignalR is used in order for the backend to send data to the frontend in real-time to all clients that are signed into the application. The frontend connects to the backend with the signalR service and receives data as soon as the backend gets it.
- Azure is used to host the database on the cloud, but also the frontend as a website that everyone can access.
- Backend still has work that needs to be done in order to be host-ready on azure, but still works if ran locally. 

Repos:
- Frontend: https://github.com/giarreh/BobsRacing-Frontend/
- Backend: https://github.com/StevenTPh/BobsRacing-Backend/

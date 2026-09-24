This project is a web API using dotnet MVC 8.

You can just run it from your Visual Studio IDE.
Migrations will be done automatically at start or if the database is deleted.
Seed data will also automatically build up a data-set for each class as soon as it becomes empty.
The app uses 2 separate sql server databases : 1 for identity, 1 for the financial classes.

There are 6 financial classes which lead to CRUD operations on them.
Identity has been role configured so that Member and Admin roles have separate user wrights.
While Members can only see the information, Admins on the other hand, have access to all CRUD actions.
This app uses JWT web tokens valid for 15 minutes.

At first start, please open the Login actions, and use Login - "try" and
fill up the credentials with :

admin@user.com / Passadmin,123
or
member@user.com / Passmember,123

If you are logged-in successfully, then copy the JWT character string provided by the response.
Then go up, at the very top of the page, click on the right on the green Authorize lock button, then in the value fill type in :
bearer+space+paste the JWT character string

	example : bearer jlfdkljghj45v,,:kmkl...


This project uses logfile to know who the app was used. The folder "logs" stores the logfiles. Just open the last one edited/created.


You will find a test project containing unit tests at service and repository level only. They use the real code, you can get a coverage analysis via the :
	Visual studio button called "Analyse code coverage for all tests".
	Ase these tests use the real code with an in-memory database fixture, you may run tests under 3 playlists :
	- 1st for all business services
	- 2nd for all business controllers
	- 3nd for all User class


This projects uses DTOS to safely display / create or update the business information.
Specific models are used to loggin, register update general user infos, update password.


Passwords must be : 
10 characters long at minimum, 
with 1 lower char min, 
1 capital char min, 
1 special character min, 
1 digit min.


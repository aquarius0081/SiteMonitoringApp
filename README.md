# SiteMonitoringApp
I recommend to open solution in the latest version of Visual Studio 2017 with all updates. .NET Core 2.1 should be installed on computer. Solution is written using modern technologies including .NET Core 2.1 WebApi, Angular4, MongoDB. Nuget is used as .NET package manager, npm as a JS package manager.

Deployment steps:
1. Install .NET Core 2.1
2. Install MongoDB server
3. Start mongoDB server by executing mongod.exe file
3. Create admin user in DB using MongoDB client (mongo.exe)
use admin
db.createUser(
  {
    user: "admin",
    pwd: "abc123!",
    roles: [ { role: "userAdminAnyDatabase", db: "admin" } ]
  }
)
4. Create mongodb.cfg file in the root folder of mongoDB server with the following contents:
systemLog:
  destination: file
  path: "C:\\data\\db\\log\\mongo.log"
  logAppend: true
storage:
  dbPath: "C:\\data\\db"
security:
  authorization: enabled  
5. Restart mongoDB server by executing command:
mongod.exe --config mongod.cfg
6. Deploy .NET Core web application on IIS server and start IIS site

*Note: to authenticate into monitoring settings use the following credentials:
Username: admin
Password: qwerty
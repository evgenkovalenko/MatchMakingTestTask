Running a .NET Solution Locally with Docker using Docker Compose

1️.	Prerequisites
Ensure you have the following installed:
- Docker Desktop for Windows
- Docker Compose


2️⃣ 	Running the Solution

Fork and Clone repository to your local dev environment.

Build solution with docker compose.

Open powershell and run next command:

 docker compose  -f "{Full-path-to-folder-with-code}\docker-compose.yml" -f "{Full-path-to-folder-with-code}\docker-compose.override.yml" -p ekovalenkocompose --ansi never up -d --build --remove-orphans
 
After that in Docker Desktop must be created next containers  

![GitHub Image](https://raw.githubusercontent.com/evgenkovalenko/images/refs/heads/main/dockercompose-1.png)
 
  
3️⃣ 	Testing the Application.

Once the containers are running, you are able to make http requests.

To Add MatchingSearch request need to make POST request as show in picture with postman

![GitHub Image](https://raw.githubusercontent.com/evgenkovalenko/images/refs/heads/main/AddMatchingSearch-2.png)

or from command line :
curl -X POST http://localhost:25331/match -H "Content-Type: application/json" -d "{\"UserId\":\"userid_1\"}"

For Matchmaking.Service application port 25331 was set in docker-compose.yml configuration, so it will be static.

To retreive match information GET request should be performed, it can be used browser, postman :     

![GitHub Image](https://raw.githubusercontent.com/evgenkovalenko/images/refs/heads/main/RetreiveMatchingResult-3.png)

OR command line:

curl -X GET "http://localhost:25331/match?userId=userid_1" -H "Accept: application/json"


4️⃣  To check application logs can be used Docker Desktop functionality :

![GitHub Image](https://raw.githubusercontent.com/evgenkovalenko/images/refs/heads/main/ServicesLogs-4.png)




 
 
 


# PHPWebServer
REST API to use MySQL Database via Unity

In this project I wanted to create a user account system that securly allowed users to login and have access to an inventory of in-game items.
I also wanted it to be tracked an available anywhere. To achieve this I used Unity, C#, PHP, xampp, and MySQL.

Unity/C#

I am extremely familiar with Unity so it was an easy choice to use this as the game engine. I have over 6 years of experience using
Unity and its C# features.

PHP

I previously had no experience with PHP but I was able to find learing resources online with direct example of the type of project I 
was working on so I learned how to use it and build the architecture out from the examples

Xampp/MySQL

For this specific project where players would not be accessing the server outside of the development environment I decided to keep 
everything on the local network. I have used Azure in the past to host the PHP webpage and MySQL database and since I had the 
experience with MySQL I decided to use Xampp for simplicity. 

MySQL Schema Design

I created the database schema to focus on speed and to use an index. With the index I can access any table based on the ID of the
logged in player to find their inventory or currently equipped items. I also created a simple store that had it's own table so 
that it could be updated any time from the item table. In order to ensure player's couldn't cheat I escaped variables to help 
prevent injection and also had checks in the PHP to make sure an items existed in the database before it could be equipped.


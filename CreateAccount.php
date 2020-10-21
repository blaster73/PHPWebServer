<?php

    // Connect to the database
    $db = new mysqli("HOST", "USERNAME", "PASSWD", "DBNAME");
    if ($db->connect_errno) {
        printf("Connection failed: %s\n", $mysqli->connect_error);
        exit();
    }

    // Escape strings to prevent SQL injection attack.
    $email = mysqli_real_escape_string($db, $_GET['email']);
    $username = mysqli_real_escape_string($db, $_GET['username']);
    $password = mysqli_real_escape_string($db, $_GET['password']);

    // Make sure a valid email address format is used
    if(!filter_var($email, FILTER_VALIDATE_EMAIL)){
        printf("Invalid email address");
    }
    else{

        // Hash the password and generate a salt using Argon2i
        $hashedPassword = password_hash($password, PASSWORD_ARGON2I, ['memory_cost' => 2048, 'time_cost' => 4, 'threads' => 3]);

        // Check to make sure the username doesn't already exists
        $stmt = $db->prepare("SELECT username FROM accounts WHERE username = ?");
        if($stmt == false){ printf("MySQL failed to execute");}
        $stmt->bind_param("s", $username);
        $stmt->execute();
        $un_results = $stmt->get_result();
        if($un_results->num_rows > 0){
            printf("Username already exists");
        }
        else{
            // Check to make sure the email doesn't already exists
            $stmt1 = $db->prepare("SELECT email FROM accounts WHERE email = ?");
            if($stmt1 == false){ printf("MySQL failed to execute");}
            $stmt1->bind_param("s", $email);
            $stmt1->execute();
            $em_results = $stmt1->get_result();
            if($em_results->num_rows > 0){
                printf("Email already exists");
            }
            else{
                // Send user information to accounts table for the MySQL database class
                $stmt2 = $db->prepare("INSERT into accounts (username, email, password) VALUES (?, ?, ?)");
                if($stmt2 == false){ printf("MySQL failed to execute");}
                $stmt2->bind_param("sss", $username, $email, $hashedPassword);
                $stmt2->execute();
                
                // Grab the newly created user id
                $stmt3 = $db->prepare("SELECT id FROM accounts WHERE username = ?");
                if($stmt3 == false){ printf("MySQL failed to execute");}
                $stmt3->bind_param("s", $username);
                $stmt3->execute();
                $id_results = $stmt3->get_result();
                if($id_results->num_rows > 0){
                    while($row = $id_results->fetch_assoc()){
                        $id = $row["id"];

                        // Create an inventory, equipped, and stats rows for the new player
                        $db->query("INSERT into equipped (id) VALUES ('$id')");
                        $db->query("INSERT into inventory (id) VALUES ('$id')");
                        $db->query("INSERT into stats (id) VALUES ('$id')");
                        // Equip a shirt for the player
                        $db->query("UPDATE equipped SET shirt = 22 WHERE id='$id'");
                    }
                    printf("User Added");
                }
            }
        }
    }

    

?>
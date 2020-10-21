<?php

    // Connect to the database
    $db = new mysqli("HOST", "USERNAME", "PASSWD", "DBNAME");
    if ($db->connect_errno) {
        printf("Connection failed: %s\n", $mysqli->connect_error);
        exit();
    }

    // Escape strings to prevent SQL injection attack.
    $username = mysqli_real_escape_string($db, $_GET['username']);
    $password = mysqli_real_escape_string($db, $_GET['password']);

    // Check if the email exists
    $stmt = $db->prepare("SELECT * FROM accounts WHERE username = ? LIMIT 1");
    if($stmt == false){ printf("Failed to login");}
    $stmt->bind_param("s", $username);
    $stmt->execute();
    $result = $stmt->get_result();
    if($result->num_rows > 0){
        while($row = $result->fetch_assoc()){
            $hashedPassword = $row["password"];

            if(password_verify($password, $hashedPassword)){
                printf($row["username"]. " ");
                printf($row["email"]. " ");
                printf($row["id"]. " ");
                printf("Successful login" . " ");

                // Assign player id
                $id = $row["id"];

                // Get the currency and level of the player from the stats table
                $stmt = $db->prepare("SELECT * FROM stats WHERE id = $id LIMIT 1");
                if($stmt == false){ printf("Failed to get stats");}
                $stmt->execute();
                $result = $stmt->get_result();
                if($result->num_rows > 0){
                    while($row1 = $result->fetch_assoc()){
                        printf($row1["currency"]. " ");
                        printf($row1["level"]);
                    }
                }
                else{
                    printf("No results to show");
                }




            }
            else{
                printf("Email or password incorrect");
            }
        }
    }
    else{
        printf("Email or password incorrect");
    }


?>
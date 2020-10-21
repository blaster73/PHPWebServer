<?php

    // Connect to the database
    $db = new mysqli("HOST", "USERNAME", "PASSWD", "DBNAME");
    if ($db->connect_errno) {
        printf("Connection failed: %s\n", $mysqli->connect_error);
        exit();
    }

    // Escape strings to prevent SQL injection attack.
    (int)$playerID = $_GET['playerID'];

    // Check if the email exists
    $stmt = $db->prepare("SELECT * FROM accounts WHERE id = ? LIMIT 1");
    if($stmt == false){ printf("Failed to login");}
    $stmt->bind_param("i", $playerID);
    $stmt->execute();
    $result = $stmt->get_result();
    if($result->num_rows > 0){
        while($row = $result->fetch_assoc()){
            printf($row["username"]);
        }
    }
    else{
        printf("No results to show");
    }

?>
<?php

    // Connect to the database
    $db = new mysqli("HOST", "USERNAME", "PASSWD", "DBNAME");
    if ($db->connect_errno) {
        printf("Connection failed: %s\n", $mysqli->connect_error);
        exit();
    }

    // Escape strings to prevent SQL injection attack.
    $playerID = mysqli_real_escape_string($db, $_GET['playerID']);

    // Print the stats of the player
    $stmt = $db->prepare("SELECT * FROM stats WHERE id = ? LIMIT 1");
    if($stmt == false){ printf("Failed to get stats");}
    $stmt->bind_param("s", $playerID);
    $stmt->execute();
    $result = $stmt->get_result();
    if($result->num_rows > 0){
        while($row = $result->fetch_assoc()){
            printf($row["currency"] . " ");
            printf($row["level"] . " ");
        }
    }
    else{
        printf("No results to show");
    }

    // Print the username of the player
    $stmt = $db->prepare("SELECT username FROM accounts WHERE id = ? LIMIT 1");
    if($stmt == false){ printf("Failed to get stats");}
    $stmt->bind_param("s", $playerID);
    $stmt->execute();
    $result = $stmt->get_result();
    if($result->num_rows > 0){
        while($row = $result->fetch_assoc()){
            printf($row["username"] . " ");
        }
    }
    else{
        printf("No results to show");
    }

?>
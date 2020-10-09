<?php

    // Connect to the database
    $db = new mysqli("fth-media-031", "aeonAdmin", "FalconsAdmin", "aeon_demo");
    if ($db->connect_errno) {
        printf("Connection failed: %s\n", $mysqli->connect_error);
        exit();
    }

    // Escape strings to prevent SQL injection attack.
    $action = mysqli_real_escape_string($db, $_GET['action']);
    $playerID = mysqli_real_escape_string($db, $_GET['playerID']);
    $current;

    $stmt = $db->prepare("SELECT * FROM stats WHERE id = ? LIMIT 1");
    if($stmt == false){ printf("Failed to get stats");}
    $stmt->bind_param("s", $playerID);
    $stmt->execute();
    $result = $stmt->get_result();
    if($result->num_rows > 0){
        while($row = $result->fetch_assoc()){
            $current = $row["currency"];
        }
    }
    else{
        printf("No results to show");
    }


    if($action == "read"){

        // Print the current currency
        printf($current);
    }
    else if($action == "add"){

        // Get the amount to add
        (int)$amount = $_GET['amount'];

        // Add 1 to the current currency
        $stmt = $db->prepare("UPDATE stats SET currency=$current+$amount WHERE id = ?");
        if($stmt == false){ printf("MySQL failed to execute");}
        $stmt->bind_param("i", $playerID);
        $stmt->execute();

        // Print what the new amount should be
        printf($current+$amount);

    }
    else if($action == "remove"){

        // get the amount to remove
        (int)$amount = $_GET['amount'];

        // See if this amount can be removed while remaining >= 0
        if($current - $amount < 0){
            printf("Not enough currency");
            exit();
        }
        else{
            // Subtract from the currency
            $stmt = $db->prepare("UPDATE stats SET currency=$current-$amount WHERE id = ?");
            if($stmt == false){ printf("MySQL failed to execute");}
            $stmt->bind_param("i", $playerID);
            $stmt->execute();

            // Print what the new amount should be
            printf($current-$amount);
        }
    }

?>
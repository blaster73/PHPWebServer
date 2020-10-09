<?php

    // Connect to the database
    $db = new mysqli("fth-media-031", "aeonAdmin", "FalconsAdmin", "aeon_demo");
    if ($db->connect_errno) {
        printf("Connection failed: %s\n", $mysqli->connect_error);
        exit();
    }

    $stmt = $db->prepare("SELECT id, currency FROM stats ORDER BY currency DESC LIMIT 10");
    if($stmt == false){ printf("Failed to get score");}
    $stmt->execute();
    $result = $stmt->get_result();
    if($result->num_rows > 0){
        while($row = $result->fetch_assoc()){

            $id = $row["id"];

            $stmt = $db->prepare("SELECT * FROM accounts WHERE id = $id LIMIT 10");
            if($stmt == false){ printf("Failed to get stats");}
            $stmt->execute();
            $result1 = $stmt->get_result();
            if($result1->num_rows > 0){
                while($row1 = $result1->fetch_assoc()){
                    printf($row1["username"]. ".");
                }
            }
            else{
                printf("No results to show");
            }

            printf($row["currency"]. " ");
        }
    }
    else{
        printf("No results to show");
    }


?>
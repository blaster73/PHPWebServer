<?php

    // Connect to the database
    $db = new mysqli("fth-media-031", "aeonAdmin", "FalconsAdmin", "aeon_demo");
    if ($db->connect_errno) {
        printf("Connection failed: %s\n", $mysqli->connect_error);
        exit();
    }

    // Escape strings to prevent SQL injection attack.
    $action = mysqli_real_escape_string($db, $_GET['action']);
    $storeName = mysqli_real_escape_string($db, $_GET['storeName']);
    
    if($action == "read"){
        
        // Read out the store
        $stmt = $db->prepare("SELECT * FROM $storeName");
        if($stmt == false){ printf("MySQL failed to execute");}
        $stmt->execute();
        $result = $stmt->get_result();
        // Print the store items with a period seperating index and price
        while($row = mysqli_fetch_assoc($result)){
            $index = 0;
            foreach ($row as $r)
            {
                if($index != 0){
                    printf("." . $r);
                }
                else{
                    printf($r);
                }
                $index++;
            }
            printf(" ");
        }
    }

?>
<?php

    // Connect to the database
    $db = new mysqli("fth-media-031", "aeonAdmin", "FalconsAdmin", "aeon_demo");
    if ($db->connect_errno) {
        printf("Connection failed: %s\n", $mysqli->connect_error);
        exit();
    }

    // Escape strings to prevent SQL injection attack.
    $action = mysqli_real_escape_string($db, $_GET['action']);
    (int)$playerID = $_GET['playerID'];
    
    if($action == "read"){
        
        // Read out the players inventory
        $stmt = $db->prepare("SELECT * FROM inventory WHERE id=?");
        if($stmt == false){ printf("MySQL failed to execute");}
        $stmt->bind_param("i", $playerID);
        $stmt->execute();
        $result = $stmt->get_result();
        // Print the inventory items ignoring the ID
        while($row = mysqli_fetch_assoc($result)){
            $index = 0;
            foreach ($row as $r)
            {
                if($index != 0){
                    printf($r . " ");
                }
                $index++;
            }
        }

    }
    elseif($action == "add"){

        // Grab and escape item for add
        $itemID = mysqli_real_escape_string($db, $_GET['itemID']);
        // Create a temp and find the first empty inventory slot
        $invSlot = FindEmptyInventorySlot($db);

        if($invSlot != 0){
            if(VerifyItem($db, $itemID) == true){
                if($itemID == 0){
                    printf("Item ID 0 can't be added to inventory!");
                    exit();
                }
                $stmt = $db->prepare("UPDATE inventory SET `$invSlot` = ? WHERE id=?");
                if($stmt == false){ printf("MySQL failed to execute");}
                $stmt->bind_param("ii", $itemID, $playerID);
                $stmt->execute();
                printf($itemID. " added in slot ". $invSlot);
            }
            else{
                printf("Item does not exist");
            }
        }
        else{
            pritf("Inventory full");
        }

    }
    elseif($action == "remove"){

        // Recieve the slot to remove an item
        (int)$invSlot = $_GET['invSlot'];
        
        $stmt = $db->prepare("UPDATE inventory SET `$invSlot` = 0 WHERE id=?");
        if($stmt == false){ printf("MySQL failed to execute");}
        $stmt->bind_param("i", $playerID);
        $stmt->execute();
        printf("Item in slot ". $invSlot. " removed");
        
    }
    elseif($action == "swap"){

        // Recieve the first slot to swap
        (int)$invSlot = $_GET['invSlot'];
        // Recieve the second slot to swap
        (int)$invSlot2 = $_GET['invSlot2'];

        $stmt = $db->prepare("UPDATE inventory SET `$invSlot` = (@temp:=`$invSlot`), `$invSlot` = `$invSlot2`, `$invSlot2` = @temp WHERE id=?");
        $stmt->bind_param("i", $playerID);
        $stmt->execute();
        printf("Items in slot ". $invSlot. " and slot ". $invSlot2. " swapped");

    }
    else{
        printf("Action not detected");
    }

    // Verify that the item being added exists
    function VerifyItem(mysqli $db, $targetInvItem){

        // Make sure item exists in the table and grab its information
        $stmt = $db->prepare("SELECT itemid FROM items WHERE itemid=?");
        if($stmt == false){ printf("Failed to verify item type");}
        $stmt->bind_param("s", $targetInvItem);
        $stmt->execute();
        $result = $stmt->get_result();

        if($result->num_rows > 0){
            return true;
        }
        else{
            return false;
        }

    }

    // Find the first open slot in the inventory
    function FindEmptyInventorySlot(mysqli $db){

        $stmt = $db->prepare("SELECT * FROM inventory WHERE id=?");
        if($stmt == false){ printf("Failed to verify slot type");}
        $stmt->bind_param("i", $GLOBALS['playerID']);
        $stmt->execute();
        $result = $stmt->get_result();

        // Ignoring the ID, find the find inventory slot with 0
        while($row = mysqli_fetch_assoc($result)){
            for($i = 1; $i < count($row); $i++){
                if($row[$i] == 0){
                    //printf("The first empty inventory slot is " . $i);
                    return $i;
                }
                elseif($i == count($row) - 1){
                    //printf("No open slots");
                    return 0;
                }

            }
        }

    }

?>
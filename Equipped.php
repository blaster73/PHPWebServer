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

    // Item information variables
    $itemName;
    $itemID;
    $itemType;

    // Currently equipped item id
    $equippedItemID;

    // Read out the players inventory
    if($action == "read"){
        
        $stmt = $db->prepare("SELECT * FROM equipped WHERE id=?");
        if($stmt == false){ printf("MySQL failed to execute");}
        $stmt->bind_param("i", $playerID);
        $stmt->execute();
        $result = $stmt->get_result();

        // Print the equipped items ignoring the ID
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
    elseif($action == "equip"){

        // receive the inventory slot of the item to equip 
        $invSlot = mysqli_real_escape_string($db, $_GET['invSlot']);
        // Get the equipped slot to equip an item in
        (int)$slotType = $_GET['slotType'];

        // Identify the item
        if(VerifyItem($db, $invSlot) == true){
            if(IsSlotOccupied($db) == true){

                // Get the id of the currently equipped item
                $stmt = $db->prepare("SELECT $itemType FROM equipped WHERE id=?");
                if($stmt == false){ printf("MySQL failed to execute");}
                $stmt->bind_param("i", $playerID);
                $stmt->execute();
                $result = $stmt->get_result();
                while($row = $result->fetch_assoc()){
                    $equippedItemID = $row[$itemType];
                }

                // Equip the item into the player's proper equipment slot
                $stmt = $db->prepare("UPDATE equipped SET `$itemType`=$itemID WHERE id = ?");
                if($stmt == false){ printf("MySQL failed to execute");}
                $stmt->bind_param("i", $playerID);
                $stmt->execute();

                // Place the previously equipped item into the player's inventory in the old slot
                $stmt = $db->prepare("UPDATE inventory SET `$invSlot`=$equippedItemID WHERE id = ?");
                if($stmt == false){ printf("MySQL failed to execute");}
                $stmt->bind_param("i", $playerID);
                $stmt->execute();

                printf("Item Equipped into occupied slot");
            }
            // If nothing is equipped
            else{

                // Equip the item into the player's proper equipment slot
                $stmt = $db->prepare("UPDATE equipped SET `$itemType`=$itemID WHERE id = ?");
                if($stmt == false){ printf("MySQL failed to execute");}
                $stmt->bind_param("i", $playerID);
                $stmt->execute();

                // remove the item from the inventory
                $stmt = $db->prepare("UPDATE inventory SET `$invSlot`=0 WHERE id = ?");
                if($stmt == false){ printf("MySQL failed to execute");}
                $stmt->bind_param("i", $playerID);
                $stmt->execute();

                printf("success");
            }
        }
        else{
            printf("Invalid item");
        }

    }
    elseif($action == "unequip"){

        // Get the equipped slot to unequip
        (int)$slotType = $_GET['slotType'];

        // Get the id of the currently equipped item
        $equippedIDToUnequip = GetequippedItemID($db);

        // If there is an empty slot do the transfer
        $emptyInvSlot = FindEmptyInventorySlot($db);
        if($emptyInvSlot != 0){

            // Place the item in the inventory
            $stmt = $db->prepare("UPDATE inventory SET `$emptyInvSlot`=? WHERE id = ?");
            if($stmt == false){ printf("MySQL failed to execute");}
            $stmt->bind_param("ii", $equippedIDToUnequip, $playerID);
            $stmt->execute();

            // Remove the item from equipped
            $stmt = $db->prepare("UPDATE equipped SET `$slotType`=0 WHERE id = ?");
            if($stmt == false){ printf("MySQL failed to execute");}
            $stmt->bind_param("i", $playerID);
            $stmt->execute();

            printf("success");

        }
        else{
            printf("Inventory is full!");
        }
        
    }
    else{
        printf("Action not detected");
    }

    //****************//
    // Useful Methods //
    //****************//

    // Verify that the item attempting to be placed in a type of slot can be placed there
    function VerifyItem(mysqli $db, $verifyInvSlot){

        // Get the item id from sent inventory slot
        $stmt = $db->prepare("SELECT `$verifyInvSlot` FROM inventory WHERE id=?");
        if($stmt == false){ printf("Failed to verify slot type");}
        $stmt->bind_param("i", $GLOBALS['playerID']);
        $stmt->execute();
        $result = $stmt->get_result();
        while($row = $result->fetch_assoc()){
            // If nothing is in the slot stop here
            if($row[$verifyInvSlot] == 0){
                printf("Inventory slot is empty!");
                exit();
            }
            $targetInvItem = $row[$verifyInvSlot];
        }

        // Make sure item exists in the table and grab its information
        $stmt = $db->prepare("SELECT * FROM items WHERE itemid=?");
        if($stmt == false){ printf("Failed to verify item type");}
        $stmt->bind_param("s", $targetInvItem);
        $stmt->execute();
        $result = $stmt->get_result();

        if($result->num_rows > 0){
            while($row = $result->fetch_assoc()){
                $GLOBALS['itemName'] = $row["name"];
                $GLOBALS['itemID'] = $row["itemid"];
                // If this is a mod grab slot type from input
                if($row["type"] != "mod"){
                    $GLOBALS['itemType'] = $row["type"];
                }
                else{
                    $GLOBALS['itemType'] = $GLOBALS['slotType'];
                }
            }
            // Make sure the type is correct for the desired slot
            if($GLOBALS['itemType'] != $GLOBALS['slotType']){
                printf("Error 307: Item type doesn't belong in this slot!");
                return false;
            }
            return true;
        }
        else{
            printf("Error 306: Item does not exist in archives!");
            return false;
        }
    }

    // Find out if anything is already equipped in the intended slot
    function IsSlotOccupied(mysqli $db){

        $tempSlot = $GLOBALS['itemType'];

        $stmt = $db->prepare("SELECT * FROM equipped WHERE id=?");
        if($stmt == false){ printf("Failed to verify slot type");}
        $stmt->bind_param("i", $GLOBALS['playerID']);
        $stmt->execute();
        $result = $stmt->get_result();

        if($result->num_rows > 0){
            // Assign the information from the item
            while($row = mysqli_fetch_assoc($result)){
                if($row[$tempSlot] == 0){
                    return false;
                }
                else{
                    return true;
                }
            }
        }
        else{
            printf("Error 308: Failed to verify equipped slot");
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

    // Get the id of the currently equipped item
    function GetequippedItemID(mysqli $db){

        $tempSlotType = $GLOBALS['slotType'];

        $stmt = $db->prepare("SELECT * FROM equipped WHERE id=?");
        if($stmt == false){ printf("MySQL failed to execute");}
        $stmt->bind_param("i", $GLOBALS['playerID']);
        $stmt->execute();
        $result = $stmt->get_result();

        if($result->num_rows > 0){
            while($row = mysqli_fetch_assoc($result)){
                return $row[$tempSlotType];
            }
        }

    }

    // Debug the item
    function DebugItem(){
        printf($GLOBALS['itemName']);
        printf($GLOBALS['itemID']);
        printf($GLOBALS['itemType']);
    }

?>
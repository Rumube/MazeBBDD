<?php

	require 'ConnectionSettings.php';

	//Variables submited by user
	$nick = $_POST["nick"];
	$password = $_POST["password"];


	if(!$conn) {
		echo "la conexión no estaría funcionando<br/>";
		die("No pudo conectarse: " . $conn->connect_error);

	}else{

		//Preventing sql injections
		$statement = $conn->prepare("SELECT * FROM table_user WHERE NICK = ?");
		$statement->bind_param("s",$nickUser);
		$statement->execute();

		$result = $statement->get_result();

		$statement->close();

		if($result->num_rows > 0){
			echo " Username is already taken ";
			
		}else{
			//Preventing sql injections
			$statement = $conn->prepare("INSERT INTO table_user(NICK,PASSWORD,CURRENT_POINTS,GLOBAL_POINTS) VALUES(?,?,?,?)");
			$globalPoints = 0;
			$currentPoints = 0;
			$statement->bind_param('ssii',$nick,$password,$currentPoints,$globalPoints);
			
			if($statement->execute()){
				echo "New user has been created";
			}else{
				echo "Error: " .$statement . "<br>" . $conn->error;
			}
			$statement->close();
		}
		
		$conn->close();
	}
?>
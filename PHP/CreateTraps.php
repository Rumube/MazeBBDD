<?php

require 'ConnectionSettings.php';


//Variables submited by user
$idMaze = $_GET["MAZE"];

	if(!$conn) {
	echo "la conexión no estaría funcionando<br/>";
	die("No pudo conectarse: " . $conn->connect_error);

	}else{
		//Preventing sql injections
			$statement = $conn->prepare("INSERT INTO table_traps(DEAD,MAZE) VALUES(?,?)");
			$dead = 0;
			$statement->bind_param('ii',$dead, $idMaze);

			if($statement->execute()){
				//Devolver la trampa
				echo "New trap has been created";
			}else{
				echo "Error: " .$statement . "<br>" . $conn->error;
			}
			$statement->close();
			$conn->close();		
	}
?>
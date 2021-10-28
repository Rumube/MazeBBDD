<?php

require 'ConnectionSettings.php';

//Variables submited by user
$idMaze = $_GET["idMaze"];

if(!$conn) {
echo "la conexión no estaría funcionando<br/>";
die("No pudo conectarse: " . $conn->connect_error);

}else{
	//Preventing sql injections
	$statement = $conn->prepare("SELECT ID,MESSAGE,USER,POSITION,CHUNK,DATE FROM table_message WHERE MAZE = ?");

	$statement->bind_param("i",$idMaze);
	$statement->execute();

	$result = $statement->get_result();

	$statement->close();
	
	if($result->num_rows > 0){
		$data = array();
		while($obj = $result->fetch_object()){
			$data[]=$obj;
		}
		echo json_encode($data);
	}else{
		echo " There isn't messages";
	}
	
	$conn->close();
}
?>
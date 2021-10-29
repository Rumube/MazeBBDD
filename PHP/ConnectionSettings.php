<?php
	//Data from database
	$serverName = "127.0.0.1";
	$db = "themaze";
	$uid = "root";
	$pwd = "";

	$conn = new mysqli( $serverName, $uid, $pwd, $db);
	
	if(!$conn) {
		echo "la conexión no estaría funcionando<br/>";
		die("No pudo conectarse: " . $conn->connect_error);
	}
?>
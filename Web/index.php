<?php
if(!isset($_POST['client']) || $_POST['client']=="")
{
	echo "Welcome!<br>
	<a>Project home</a>";
	die();
}
else
{
	//echo "Client data";
	$conn = mysqli_connect("mysql.hostinger.hu", "u821146919_games", "SLp7q9m60J", "u821146919_games");
	if($_POST['client']=="cheesecrescent")
	{ //GameList
		$res=mysqli_query($conn, "SELECT * FROM games");
		while($row=mysqli_fetch_array($res))
		{
			if(strlen($row['playernames'])==0)
				$namecount=0;
			else
				$namecount=count(explode(',', $row['playernames']));
			//echo ("Startdate: ".$row['startdate']." Time-1hr: ".(time()-60*60*1000));
			if($namecount==0 || strval($row['startdate'])<time()-60*60*1000)
			{
				mysqli_query($conn, "DELETE FROM games
				WHERE name='".$row['name']."'
				AND ownername='".$row['ownername']."'
				AND maxplayers='".$row['maxplayers']."'
				AND playernames='".$row['playernames']."'
				AND startdate='".$row['startdate']."'
				AND ip='".$row['ip']."'
				") or die(mysqli_error($conn));
			}
			else
			{
				echo $row['name']."|".$row['ownername']."|".$row['maxplayers']."|".$namecount."|".$row['playernames']."|".$row['ip']."|";
			}
		}
	}
	else
	{
		if($_POST['name']=="" || $_POST['maxplayers']=="" || $_POST['client']=="" || $_POST['ip']=="" || $_POST['action']=="")
			die("A field is or more fields are empty!");
		if($_POST['action']=="create")
		{
			mysqli_query($conn, "DELETE FROM games
			WHERE name='".$_POST['name']."'
			AND ownername='".$_POST['client']."'
			AND ip='".$_POST['ip']."'
			") or die(mysqli_error($conn));
			mysqli_query($conn, "INSERT INTO games
			(
			name, maxplayers, playernames, ownername, startdate, ip
			) VALUES (
			'".$_POST['name']."', '".$_POST['maxplayers']."', '".$_POST['client']."', '".$_POST['client']."', '".time()."', '".$_POST['ip']."'
			)") or die(mysqli_error($conn));
			echo "OK";
		}
		else if($_POST['action']=="change")
		{
			mysqli_query($conn, "UPDATE games SET playernames='".$_POST['playernames']."'
			WHERE name='".$row['name']."'
			AND ownername='".$row['ownername']."'
			AND ip='".$row['ip']."'
			") or die(mysqli_error($conn));
			echo "OK";
		}
		else if($_POST['action']=="remove")
		{ //Creating a new game on the same IP, with the same playername and same gamename will also remove. (^^)
			mysqli_query($conn, "DELETE FROM games
			WHERE name='".$_POST['name']."'
			AND ownername='".$_POST['client']."'
			AND ip='".$_POST['ip']."'
			") or die(mysqli_error($conn));
			echo "OK";
		}
	}
}
//WebPass: vhwfsQOv9E
//fötöpö: (...sznpadmin) VKiSh7i1og

?>

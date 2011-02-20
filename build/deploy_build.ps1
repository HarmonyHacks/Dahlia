properties {
	$deploy_dir = "C:\InetPub\wwwroot\Dahlia"
}

task right_click_deploy {
	if($lastExitCode -eq 0) {
		Get-ChildItem $build_output_dir | ForEach-Object { Copy-Item $_.FullName $deploy_dir }
	}
}
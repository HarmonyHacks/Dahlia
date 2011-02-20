properties {
	$deploy_dir = "C:\InetPub\wwwroot\Dahlia"
}

task right_click_deploy {
	if($lastExitCode -eq 0) {
		Copy-Item $build_output_dir $deploy_dir
	}
}
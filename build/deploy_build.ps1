properties {
	$publish_dir = "$build_dir\Publish"
	$deploy_dir = "C:\InetPub\wwwroot\Dahlia"
	$web_proj_file = "$base_dir\src\Dahlia\Dahlia.csproj"
}

task right_click_deploy {
    & $msbuild $web_proj_file /p:WebProjectOutputDir="$publish_dir\" `
				/p:OutputPath="$publish_dir\bin" `
				/p:Configuration="Release" `
				/p:Debug=false `
				/p:UseWPP_CopyWebApplication=false `
				/t:"ResolveReferences;Compile;_CopyWebApplication"  
                
	if($lastExitCode -eq 0) {
		Get-ChildItem $publish_dir | ForEach-Object { Copy-Item $_.FullName $deploy_dir }
	}
}
properties {
	$testrpt_dir = "$build_dir\TestReports"
	$testrpt_name = "TestReport"
	$specs = @('RIS.CharlesRiver.Tests.dll') 
	$tests = @('RIS.CharlesRiver.Tests.dll') 
					  #Note: We profile test assemblies to identify dead test code. 
	$profile_assemblies = "Ris.CharlesRiver.*"
	
	$mspec = "$base_dir\ThirdParty\mspec\mspec.exe"
	$mspec_options = "--teamcity"
	$nunit = "$base_dir\packages\NUnit.2.5.7.10213\Tools\nunit-console-x86.exe"
	$ncover3 = "C:\Program Files (x86)\NCover"
	$ncover3Runner = "$ncover3\NCover.Console.exe"
	$ncover3Reporting = "$ncover3\NCover.Reporting.exe"
	$mspecCoverageReport = "mspeccoverageReport.xml"
	$nunitCoverageReport = "nunitcoverageReport.xml"
	$nunitReport = "$testrpt_dir\nunitTestReport.xml"
}

task test -depends compile, copysqlite, test_teamcity 

task test_teamcity {

  if ($tests.Length -le 0) { 
     Write-Host -ForegroundColor Red 'No tests defined'
     return 
  }

  if ($specs.Length -le 0) { 
     Write-Host -ForegroundColor Red 'No specs defined'
     return 
  }
  
  if (Test-Path $testrpt_dir) { Remove-Item -Recurse -Force $testrpt_dir }
  New-Item $testrpt_dir -ItemType directory | Out-Null

  # mspec
  $spec_assemblies = $specs | ForEach-Object { "$build_output_dir\$_" }

  & $ncover3Runner $mspec $mspec_options $spec_assemblies '//reg' '//a' $profile_assemblies '//x' "'$testrpt_dir\$mspecCoverageReport'" '//w' $build_output_dir

    if($lastExitCode -ne 0) {
		throw "Tests Failed."
	}

  # nunit
  $test_assemblies = $tests | ForEach-Object { "$build_output_dir\$_" }

  & $ncover3Runner $nunit $test_assemblies /noshadow /xml=$nunitReport '//reg' '//a' $profile_assemblies '//x' "'$testrpt_dir\$nunitCoverageReport'"

    if($lastExitCode -ne 0) {
		throw "Tests Failed."
	}
	
	#coverage reporting
	& $ncover3Reporting $testrpt_dir"\$mspecCoverageReport" $testrpt_dir"\$nunitCoverageReport" //or FullCoverageReport:Html //op $testrpt_dir\CoverageReport //p 'Conduit - Current'
	
    Write-Host "##teamcity[dotNetCoverage ncover3_home='$ncover3']"
    Write-Host "##teamcity[dotNetCoverage ncover3_reporter_args='//or Summary:Html:{teamcity.report.path}']"
	Write-Host "##teamcity[importData type='dotNetCoverage' tool='ncover3' path='$testrpt_dir\$mspecCoverageReport']"  
	Write-Host "##teamcity[importData type='dotNetCoverage' tool='ncover3' path='$testrpt_dir\$nunitCoverageReport']"  
	Write-Host "##teamcity[importData type='nunit' path='$nunitReport']"

	Write-Host "Finished specs and tests"
}

task copysqlite {
	#Copy-Item -Force "$base_dir\$sqlite" $build_output_dir
}
properties {
	$testrpt_dir = "$build_dir\TestReports"
	$testrpt_name = "TestReport"
	$specs = @('Dahlia.Specifications.dll') 
	$tests = @('Dahlia.Tests.dll') 
					  #Note: We profile test assemblies to identify dead test code. 
	$profile_assemblies = "Ris.CharlesRiver.*"
	
	$mspec = "$tool_dir\mspec\mspec.exe"
	$mspec_options = "--teamcity"
	$nunit = "$tool_dir\NUnit.2.5.7.10213\Tools\nunit-console-x86.exe"
	$nunitReport = "$testrpt_dir\nunitTestReport.xml"
	
}

task test -depends compile, test_teamcity 

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

  & $mspec $mspec_options $spec_assemblies  

    if($lastExitCode -ne 0) {
		throw "Tests Failed."
	}

  # nunit
  $test_assemblies = $tests | ForEach-Object { "$build_output_dir\$_" }

  & $nunit $test_assemblies /noshadow /xml=$nunitReport

    if($lastExitCode -ne 0) {
		throw "Tests Failed."
	}
	
	
    Write-Host "##teamcity[importData type='nunit' path='$nunitReport']"

	Write-Host "Finished specs and tests"
}
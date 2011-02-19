properties {
$build_number = if($env:BUILD_NUMBER) {$env:BUILD_NUMBER.Split('.')[2] } else { "0" }
$version = if($env:BUILD_NUMBER) {$env:BUILD_NUMBER} else { "1.0.0.0" }
}

include .\Build\master_build.ps1
include .\Build\test_build.ps1
task default -depends compile #, test
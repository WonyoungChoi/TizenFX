#!groovy

import groovy.transform.Field

@Field def BRANCH_API_MAP = [master: 'API6', API5: 'API5', API4: 'API4']
@Field def ACR_REQUIRED = 'ACR Required'
@Field def INTERNAL_API_CHANGED = 'Internal API Changed'

void setAPILevel() {
  def apiLevel = BRANCH_API_MAP[pullRequest.base]
  if (apiLevel) {
    addLabel(apiLevel)
  }
}

void build(Map config) {
  // checkout
  if (config.checkout) {
    sh "git checkout ${config.checkout}"
  }

  // clean
  if (config.clean) {
    sh './build.sh clean'
  }

  // build
  def buildCmd = './build.sh full'
  if (config.analysis) {
    buildCmd += ' /p:BuildWithAnalyzer=True | tee build.log'
  }
  sh buildCmd

  // pack
  if (config.pack) {
    sh './build.sh pack'
  }
}

void reportAnalysis(Map config) {
  withCredentials([string(credentialsId: config.credentialsId, variable: 'TOKEN')]) {
    if (fileExists('tools/scripts/CodeChecker/main.py')) {
      sh "python tools/scripts/CodeChecker/main.py ${TOKEN} ${pullRequest.number}"
    }
  }
}

void checkABI(Map config) {
  def abiCheckerPath = './tools/bin/ABIChecker/ABIChecker.dll'
  if (!fileExists(abiCheckerPath)) {
    abiCheckerPath = './tools/ABIChecker/Checker_ABI.dll'
  }

  def ret = sh(returnStatus: true, script: "dotnet ${abiCheckerPath} -b ${config.basePath} -p ${config.prPath}")
  switch (ret) {
    case 0:
      removeLabel ACR_REQUIRED
      removeLabel INTERNAL_API_CHANGED
      break;
    case 1:
      removeLabel ACR_REQUIRED
      addLabel    INTERNAL_API_CHANGED
      break;
    case 2:
      addLabel    ACR_REQUIRED
      removeLabel INTERNAL_API_CHANGED
      break;
    case 3:
      addLabels   [ACR_REQUIRED, INTERNAL_API_CHANGED]
      break;
  }
}

void addLabel(label) {
  addLabels([label])
}

void addLabels(labels) {
  try {
    pullRequest.addLabels(labels)
  } catch (err) {
  }
}

void removeLabel(label) {
  try {
    pullRequest.removeLabel(label)
  } catch (err) {
  }
}


return this

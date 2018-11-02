pipeline {
  agent any
  stages {
    stage('API Level') {
      when {
        changeRequest()
      }
      steps {
        script {
          if (pullRequest.base == "master") {
            pullRequest.addLabels("API6")
          } else if (pullRequest.base == "API5") {
            pullRequest.addLabels("API5") 
          }
        }
      }
    }
    stage ('Commit Status') {
      when {
        changeRequest()
      }
      steps {
        script {
          pullRequest.createStatus(status: 'success',
                                   context: 'TizenFX/BuildChecker',
                                   description: 'FInished!')
        }
      }
    }
  }
}

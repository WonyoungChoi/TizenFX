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
            echo "API6 !!!" 
          }
        }
      }
    }
  }
}

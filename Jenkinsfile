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
            echo "This is API6!!"
          } else if (pullRequest.base == "API5") {
            echo "This is API5!!"
          }
        }
      }
    }
  }
}

pipeline {
  agent any
  stages {
    stage('API Level') {
      when {
        changeRequest()
      }
      steps {
        script {
          echo "Test !!"
        }
      }
    }
  }
}

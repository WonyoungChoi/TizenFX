pipeline {
  agent any
  stages {
    stage('Check Branch') {
      steps {
        sh '''#!/bin/sh

env'''
      }
    }
    stage('I am Hacker') {
      parallel {
        stage('I am Hacker') {
          steps {
            echo 'I am Hacker'
          }
        }
        stage('Hello B') {
          steps {
            echo 'Hello B'
          }
        }
        stage('Hello C') {
          steps {
            echo 'Hello C'
          }
        }
      }
    }
  }
}

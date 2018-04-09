pipeline {
  agent any
  stages {
    stage('Check Branch') {
      steps {
        sh '''#!/bin/sh

env'''
      }
    }
    stage('Hello A') {
      parallel {
        stage('Hello A') {
          steps {
            echo 'Hello A'
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
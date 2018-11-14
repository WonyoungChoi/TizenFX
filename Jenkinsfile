node('Build_Worker') {


  stage('SCM') {
    checkout scm
  }

  if (env.CHANGE_ID) {
    pullRequest.addLabel('TEST_LABEL')
  }

  def myTest = load('build/ci-scripts/myTest.groovy')

  stage('MyTest') {
    echo 'Hello Jenkins'
    myTest.sayHello()
  }

}

node('Build_Worker') {

  checkout scm
  
  sh 'ls -al'
  
  
  def myTest = load('build/ci-scripts/myTest.groovy')
  
  echo 'Hello Jenkins'
  myTest.sayHello()

}

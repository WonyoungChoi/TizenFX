node('Build_Worker') {

  sh 'ls -al'
  
  
  def myTest = load('build/ci-scripts/myTest.groovy')
  
  echo 'Hello Jenkins'
  myTest.sayHello()

}

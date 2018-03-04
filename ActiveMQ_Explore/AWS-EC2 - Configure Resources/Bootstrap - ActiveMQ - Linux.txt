sudo yum update -y

sudo yum install java-1.8.0 -y

sudo yum remove java-1.7.0-openjdk -y

mkdir downloads

mkdir active-mq-server

cd downloads

wget https://repo1.maven.org/maven2/org/apache/activemq/apache-activemq/5.15.3/apache-activemq-5.15.3-bin.tar.gz

cd ../active-mq-server

tar zxvf /home/ec2-user/downloads/apache-activemq-5.15.3-bin.tar.gz

cd apache-activemq-5.15.3/bin

./activemq console

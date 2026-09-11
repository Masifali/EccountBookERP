#!/bin/bash

# Stop the existing Java process
pkill -f filehandler.jar

# Start the application again
java -jar filehandler.jar
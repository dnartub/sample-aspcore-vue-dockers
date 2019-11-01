#!/bin/bash

while ! curl -v --silent http://elk.service:9200/_cat/health 2>&1 | tr ' ' '\n' | grep  'elasticsearch'; do 
	sleep 5; 
done
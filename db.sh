#!/usr/bin/env sh
sudo -u postgres psql
create database Metatron;
create user Metatron with encrypted password 'D3m1urg3';
grant all privileges on database Metatron to Metatron;

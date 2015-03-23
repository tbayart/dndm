# Internet Explorer #

Cookie location: %USERPROFILE%\AppData\Roaming\Microsoft\Windows\Cookies

Microsoft decided to make it a little more difficult to load the cookies. So first off they randomize the cookie file names so you, or at least they want you to believe, can't correlate cookie to domain. This is quite interesting so that bad guys can't steal your logins.

Well, if they randomize the file names, then they need a way to point the domain back to the file. This is contained in %USERPROFILE%\AppData\Roaming\Microsoft\Windows\Cookies\Low\index.dat

I am still not clear on the exact format, but you can see what I did in CookieManager.cs

## Windows API ##

Microsoft does provide API to manage cookies. However, after doing some digging it appears that this API only allows cookies per application. This does not seem to allow a developer to pull cookies that Internet Explorer has set - which is the reason for the hack.

# Chrome #

Cookie location: %USERPROFILE%\AppData\Local\Google\Chrome\User Data\Default

Both chrome and firefox use SQLite to store cookie information. This in itself presented a rather interesting problem. If they are open and using the SQLite database - it doesn't seem to allow other applications to open the file - even for a select. So in my code I copy the database to Cookies.dndm and read from that file. Then I delete the temporary file.

Chrome stores its cookies in a table called cookies.

Here is the DDL for that table:

```
CREATE TABLE cookies ( 
    creation_utc    INTEGER NOT NULL
                            UNIQUE
                            PRIMARY KEY,
    host_key        TEXT    NOT NULL,
    name            TEXT    NOT NULL,
    value           TEXT    NOT NULL,
    path            TEXT    NOT NULL,
    expires_utc     INTEGER NOT NULL,
    secure          INTEGER NOT NULL,
    httponly        INTEGER NOT NULL,
    last_access_utc INTEGER NOT NULL,
    has_expires     INTEGER NOT NULL
                            DEFAULT 1,
    persistent      INTEGER NOT NULL
                            DEFAULT 1 
);
```

Chrome stores it's expires in milliseconds instead of seconds. In order to convert you just need to perform this calculation:

( expires\_utc / 1000000 ) - 11644473600

# Firefox #

Cookie Location: %USERPROFILE%\AppData\Roaming\Mozilla\Firefox\Profiles\?????

(You will need to inspect profiles.ini to get the users directories)

Firefox stores it's cookies in a table called moz\_cookies

Here is it's DDL

```
CREATE TABLE moz_cookies ( 
    id           INTEGER PRIMARY KEY,
    baseDomain   TEXT,
    name         TEXT,
    value        TEXT,
    host         TEXT,
    path         TEXT,
    expiry       INTEGER,
    lastAccessed INTEGER,
    creationTime INTEGER,
    isSecure     INTEGER,
    isHttpOnly   INTEGER,
    CONSTRAINT moz_uniqueid UNIQUE ( name, host, path ) 
);
```

# Leaking Cookies #

You may notice in my code I am very liberal with my queries for Chrome and Firefox. Fear not - when the backend process makes the connection it should be smart enough to only expose cookies that are specific to that domain.
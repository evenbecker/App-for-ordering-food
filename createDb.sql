USE foodshop;
DROP TABLE IF EXISTS EORDERLINE;
DROP TABLE IF EXISTS EORDER;
DROP TABLE IF EXISTS ESTOCK;
DROP TABLE IF EXISTS EMEMBER;

CREATE TABLE EMEMBER (
	memberno INT(6) NOT NULL auto_increment,
	forename VARCHAR(20),
	surname VARCHAR(20),
    username VARCHAR(30),
    password VARCHAR(40),
	street VARCHAR(40),
	town VARCHAR(20),
	postcode VARCHAR(10),
	email VARCHAR(40),
	category VARCHAR(6),
	PRIMARY KEY (memberno)
);

INSERT INTO EMEMBER (memberno, forename, surname, username, password, street, town, postcode, email, category) 
VALUES 
( '123980','Jean','Dujardin','jdujardin','5f4dcc3b5aa765d61d8327deb882cf99','12 rue du jardin','Paris','PH3 WE4','j.dujardin@nomail.com','gold'),
( '345637','Anne','Weber','aweber','5f4dcc3b5aa765d61d8327deb882cf99','7 rue George','Paris','PH1 4ER','anne.weber@yahoo.fr','silver'),
( '659000','Alice','Lambert','alambert','5f4dcc3b5aa765d61d8327deb882cf99','101 rue du marché','Paris','PH2 3ZX','alice.lambert@hotmail.com','gold'),
( '231901','Teresa','Dupont','tdupont','5f4dcc3b5aa765d61d8327deb882cf99','4 allée des fleurs','Nantes','DD1 RT5','t.dupont@yahoo.fr','bronze');

create table ESTOCK(
	stockno VARCHAR(5) NOT NULL,
	description VARCHAR(40),
	price DECIMAL(10,2),
	qtyinstock INT(6),
	PRIMARY KEY (stockno)
);

INSERT INTO ESTOCK (stockno, description, price, qtyinstock)
VALUES
('EG334', 'Truffes', 600.00, 20),
('HG602', 'Noix', 200.00, 50),
('SH990', 'Broccoli', 35.00, 100),
('SP120', 'Raisin', 500.00, 3),
('WS980', 'Figues', 350.00, 40),
('GD500', 'Melon', 250.00, 40),
('GD550', 'Echalottes', 300.00, 40);

create table EORDER(
	orderno INT(8) AUTO_INCREMENT NOT NULL,
	memberno INT(6) NOT NULL,
	PRIMARY KEY (orderno),
	FOREIGN KEY (memberno) REFERENCES EMEMBER(memberno)
);

CREATE TABLE EORDERLINE (
	orderno INT(8) NOT NULL,
	stockno VARCHAR(5) NOT NULL,
    quantity INT(6),
	PRIMARY KEY (orderno,stockno),
	FOREIGN KEY (orderno) REFERENCES EORDER(orderno),
	FOREIGN KEY (stockno) REFERENCES ESTOCK(stockno)
);





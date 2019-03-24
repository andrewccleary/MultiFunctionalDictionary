CREATE TABLE WORDMAP (
	Word VARCHAR(50) NOT NULL,
	VerseId INT NOT NULL,
	ReferenceNum INT NOT NULL,
	PRIMARY KEY (Word, VerseId),
	FOREIGN KEY (VerseId) REFERENCES BIBLE(VerseId),
	FOREIGN KEY (ReferenceNum) REFERENCES TRANSLATION(ReferenceNum)
);
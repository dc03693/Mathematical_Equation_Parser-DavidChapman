for %%x in (*.jar) do (
	echo %%x %* >> log.txt
	java -jar %%x "1+1" %* >> log.txt
	java -jar %%x "1+2+1" %* >> log.txt
	java -jar %%x "(1+1)" %* >> log.txt
	java -jar %%x "(1)" %* >> log.txt
	java -jar %%x "((1))" %* >> log.txt
	java -jar %%x "2*v" %* >> log.txt
	java -jar %%x "(2-5)/r" %* >> log.txt
	java -jar %%x "(11*6-((((m)))))" %* >> log.txt
	java -jar %%x "(var-t+15)" %* >> log.txt
	java -jar %%x "(Var*6)-(0.001)" %* >> log.txt

	java -jar %%x "1++1" %* >> log.txt
	java -jar %%x "1+*+1" %* >> log.txt
	java -jar %%x "(1(" %* >> log.txt
	java -jar %%x "()(1)" %* >> log.txt
	java -jar %%x "2*v(" %* >> log.txt
	java -jar %%x "2-5)/r" %* >> log.txt
	java -jar %%x "(-11*6-((((m)))))" %* >> log.txt
	java -jar %%x "(var-((t+15)" %* >> log.txt
	java -jar %%x "(Var*6)-(0.001.2)" %* >> log.txt
	java -jar %%x "(1CaR-9)/0.001" %* >> log.txt
	echo. >>log.txt
)

for %%x in (*.exe) do (
	echo %%x %* >> log.txt
	%%x "1+1" %* >> log.txt
	%%x "1+2+1" %* >> log.txt
	%%x "(1+1)" %* >> log.txt
	%%x "(1)" %* >> log.txt
	%%x "((1))" %* >> log.txt
	%%x "2*v" %* >> log.txt
	%%x "(2-5)/r" %* >> log.txt
	%%x "(11*6-((((m)))))" %* >> log.txt
	%%x "(var-t+15)" %* >> log.txt
	%%x "(Var*6)-(0.001)" %* >> log.txt

	%%x "1++1" %* >> log.txt
	%%x "1+*+1" %* >> log.txt
	%%x "(1(" %* >> log.txt
	%%x "()(1)" %* >> log.txt
	%%x "2*v(" %* >> log.txt
	%%x "2-5)/r" %* >> log.txt
	%%x "(-11*6-((((m)))))" %* >> log.txt
	%%x "(var-((t+15)" %* >> log.txt
	%%x "(Var*6)-(0.001.2)" %* >> log.txt
	%%x "(1CaR-9)/0.001" %* >> log.txt
	echo. >>log.txt
)
pause
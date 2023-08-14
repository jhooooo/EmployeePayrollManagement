Sprout Solutions automates all the administrative tasks around HR and Payroll

## Description

This repository is a simple Employee and Payroll management for demonstration purposes.

### Setting up the project

1. Restore db using SproutExamDB08142023.bak file
2. Change connection string in appsettings.json
3. Install packages using 'npm install' command
4. Run the program

## Next project improvement

>> Employee Management

- IsDeleted can be changed to EmploymentStatus
- Add pagination on employee page
- Add search functionality for further filtering of data

>> Payroll Management

- There should be a page to maintain employee payroll details (salary rate, tax, etc)	 
- Absences days/Worked days can be pulled out from leave/attendance system instead of manual input
- Working days in a month is not always 22. Can add reference table for number of working days in a month
- Computation sample for regular. Should we only apply tax for taxable income (after absences deduction)? 
- Calculations can be improved based on overtime, other deductions, bonuses



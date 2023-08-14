import React, { Component } from 'react';
import authService from '../../components/api-authorization/AuthorizeService';
import { defaultOptions, currencyFormatter } from '../../components/Common.js';

export class EmployeeCalculate extends Component {
  static displayName = EmployeeCalculate.name;

  constructor(props) {
    super(props);
      this.state = { id: 0, fullName: '', birthdate: '', tin: '', typeId: 1, salaryrate: 0, tax: 0, absentDays: 0, workedDays: 0, netIncome: 0, loading: true, loadingCalculate: false, errors: {} };
  }

  componentDidMount() {
    this.getEmployeePayroll(this.props.match.params.id);
  }
  handleChange(event) {
    this.setState({ [event.target.name] : event.target.value});
  }

  handleSubmit(e){
      e.preventDefault();
      if (this.handleValidation()) {
          this.calculateSalary();
      }
    }

    handleValidation() {
      let errors = {};
      let formIsValid = true;

      //contractual worked days
      if (this.state.typeId == 2 && (this.state.workedDays > 22 || this.state.workedDays < 0)) {
          formIsValid = false;
          errors["workedDays"] = "Value must be within 22 working days";
      }

      if (this.state.typeId == 2 && (this.state.workedDays % 0.5 !== 0)) {
          formIsValid = false;
          errors["workedDays"] = "Input is not a valid worked days value";
      }

        //regular absent days
        if (this.state.typeId == 1 && (this.state.absentDays > 22 || this.state.absentDays < 0)) {
            formIsValid = false;
            errors["absentDays"] = "Value must be within 22 working days";
        }

        if (this.state.typeId == 1 && (this.state.absentDays % 0.5 !== 0)) {
            formIsValid = false;
            errors["absentDays"] = "Input is not a valid absent days value";
        }

      this.setState({ errors: errors });
      return formIsValid;
  }

  render() {

    let contents = this.state.loading
    ? <p><em>Loading...</em></p>
    : <div>
    <form>
<div className='form-row'>
<div className='form-group col-md-12'>
  <label>Full Name: <b>{this.state.fullName}</b></label>
</div>

</div>

<div className='form-row'>
<div className='form-group col-md-12'>
  <label >Birthdate: <b>{this.state.birthdate}</b></label>
</div>
</div>

<div className="form-row">
<div className='form-group col-md-12'>
  <label>TIN: <b>{this.state.tin}</b></label>
</div>
</div>

<div className="form-row">
<div className='form-group col-md-12'>
  <label>Employee Type: <b>{this.state.typeId === 1?"Regular": "Contractual"}</b></label>
</div>
</div>

{ this.state.typeId === 1?
 <div className="form-row">
   <div className='form-group col-md-12'><label>Salary: <b>{this.state.salaryrate}</b> </label></div>
   <div className='form-group col-md-12'><label>Tax: <b>{this.state.tax}%</b> </label></div>
   </div> : <div className="form-row">
   <div className='form-group col-md-12'><label>Rate Per Day: <b>{this.state.salaryrate} </b> </label></div>
</div> }

<div className="form-row">

{ this.state.typeId === 1? 
<div className='form-group col-md-6'>
  <label htmlFor='inputAbsentDays4'>Absent Days: </label>
  <input type='number' min='0' max='22' step="0.5" className='form-control' id='inputAbsentDays4' onChange={this.handleChange.bind(this)} value={this.state.absentDays} name="absentDays" placeholder='Absent Days' />
  <span className="error">{this.state.errors["absentDays"]}</span>
</div> :
<div className='form-group col-md-6'>
  <label htmlFor='inputWorkDays4'>Worked Days: </label>
  <input type='number' min='0' max='22' step="0.5" className='form-control' id='inputWorkDays4' onChange={this.handleChange.bind(this)} value={this.state.workedDays} name="workedDays" placeholder='Worked Days' />
  <span className="error">{this.state.errors["workedDays"]}</span>
</div>
}
</div>

<div className="form-row">
<div className='form-group col-md-12'>
   <label>Net Income: <b>{currencyFormatter(this.state.netIncome)}</b></label>
</div>
</div>

<button type="submit" onClick={this.handleSubmit.bind(this)} disabled={this.state.loadingCalculate} className="btn btn-primary mr-2">{this.state.loadingCalculate?"Loading...": "Calculate"}</button>
<button type="button" onClick={() => this.props.history.push("/employees/index")} className="btn btn-primary">Back</button>
</form>
</div>;


    return (
        <div>
        <h1 id="tabelLabel" >Employee Calculate Salary</h1>
        <br/>
        {contents}
      </div>
    );
  }

  async calculateSalary() {
    this.setState({ loadingCalculate: true });
    const token = await authService.getAccessToken();
    const requestOptions = {
        method: 'POST',
        headers: !token ? {} : { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
        body: JSON.stringify({ id: this.state.id, employeeId: this.state.employeeId, absences: this.state.absentDays, daysWorked: this.state.workedDays })
    };
    const response = await fetch('api/employeepayroll/' + this.state.id + '/calculate',requestOptions);
      const data = await response.json();
      this.setState({ loadingCalculate: false, netIncome: data.result });
  }

    async getEmployeePayroll(id) {
    this.setState({ loading: true,loadingCalculate: false });
    const token = await authService.getAccessToken();
    const response = await fetch('api/employeepayroll/' + id, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    if(response.status === 200){
        const data = await response.json();
        this.setState({ id: data.result.id, employeeId: data.result.employeeId, fullName: data.result.fullName, birthdate: data.result.birthdate, tin: data.result.tin, typeId: data.result.typeId, salaryrate: data.result.rate, tax: data.result.tax, loading: false, loadingCalculate: false });
    }
    else{
        alert("There was an error occured.");
        this.setState({ loading: false,loadingCalculate: false });
    }
  }
}

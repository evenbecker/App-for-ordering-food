import axios from 'axios';
import { useNavigate } from 'react-router-dom';

export default function RegisterPage() {
	const navigate = useNavigate();
	return (
		<div className="container">
				<div className="row">
					<div className="col">
						<form className="register-form" onSubmit={e => { 
								e.preventDefault(); 
								const form = e.target;

								const newUser = {
									username: form.username.value,
									password: form.password.value,
									email: form.email.value,
									forename: form.forename.value,
									surname: form.surname.value,
									street: form.street.value,
									town: form.town.value,
									postcode: form.postcode.value,
									category: form.category.value
								}

								axios.post("http://localhost:5247/api/users/register", newUser)
    							.then(res => { 
									alert("Registration successful!")
 									navigate("/login")})
    							.catch(err => {
									alert(err.response.data || "Registration failed");
									//console.error("Registration failed", err)
								});}}
							>
							<h5>Register To Create An Account</h5>
							<div>
								<input type="text" name="username" size="40" maxLength="40" placeholder="Username" className="mt-2 mb-2" required/>
							</div>
							<div>
								<input type="password" name="password" size="40" maxLength="40" placeholder="Password" className="mb-2" required/>
							</div>
							<div>
								<input type="email" name="email" size="40" maxLength="40" placeholder="Email" className="mb-2" required/>
							</div>
							<div>
								<input type="text" name="forename" size="40" maxLength="40" placeholder="Forename" className="mb-2" required/>
							</div>
							<div>
								<input type="text" name="surname" size="40" maxLength="40" placeholder="Surname" className="mb-2" required/>
							</div>
							<div>
								<input type="text" name="street" size="40" maxLength="40" placeholder="Street" className="mb-2" required/>
							</div>
							<div>
								<input type="text" name="town" size="40" maxLength="40" placeholder="Town" className="mb-2" required/>
							</div>
							<div>
								<input type="text" name="postcode" size="40" maxLength="40" placeholder="Postcode" className="mb-2" required/>
							</div>
								<div>
									<select name="category" className="mb-2" required defaultValue="">
										<option value="" disabled>Select Category</option>
										<option value="gold">Gold</option>
										<option value="silver">Silver</option>
										<option value="bronze">Bronze</option>
									</select>
								</div>     
							<button type="submit" className="btn btn-primary">Register</button>
						</form>
					</div>
					<div className="col">
						<div className="image">
							<img src="./img/register.png" alt="the picture of registration process" style={{width: 300 + "px"}} />
						</div>
						
					</div>		
			</div>
		</div>
	)
}
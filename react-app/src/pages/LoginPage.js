import axios from 'axios';
import { useNavigate, Link } from 'react-router-dom';

export default function LoginPage({ onLoginSuccess }) {
	const navigate = useNavigate();
    return (
            <div className="container">
				<div className="row">
					<div className="col">
						<form name="loginform" onSubmit={async e => {
								e.preventDefault();
								const form = e.target;

								const userCredentials = {
									username: form.username.value,
									password: form.password.value
								}

								try {
									const res = await axios.post('http://localhost:5247/api/users/login', userCredentials);

									const token = res.data.token;
									localStorage.setItem('token', token); 

									onLoginSuccess(); // Call the function to update the login state
									alert("Login successful!")
 									navigate("/")
								} catch (err) {
									console.error('Login error:', err);
									alert("Login failed. Please check your credentials.");
								}
						}}>
							<h5>Please Login</h5>
							<div className="username">
								<input name="username" type="text" size="40" maxLength="40" placeholder="Username" className="mb-2" required/>
							</div>
							<div className="password">
								<input name="password" type="password" size="40" maxLength="40" placeholder="Password" className="mb-2" required/>
							</div>
						    <div className="row">
						        <div className="col">
						            <div className="btn-group mb-3" role="group" aria-label="Horizontal button group">
						                <button type="submit" className="btn btn-primary mr-2" name="Submit">Submit</button>
						            </div>
						        </div>
						    </div>
						</form>
						<p>
  							Don't have an account? <br /> Please {' '}
							<Link to="/register">Register</Link>
						</p>
					</div>
					<div className="col">
						<div className="image">
							<img src="./img/login.png" alt="unable to load image" style={{width: 300 + "px"}} />
						</div>
						
					</div>		
			</div>
		</div>
    )
}
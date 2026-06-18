import { createContext,useContext,useState } from "react"
import { type AuthResponse} from "../types"

interface AuthContextType{
    user: AuthResponse | null,
    login: (data: AuthResponse)=>void,
    logout: ()=> void,
    isAuthenticated: boolean
}

const AuthContext = createContext<AuthContextType | null>(null)

export function AuthProvider({children}: {children: React.ReactNode}){
    const [user, setUser] = useState<AuthResponse | null>(null)

    const login = (data: AuthResponse)=>{
        setUser(data)
        localStorage.setItem("token", data.token)
        localStorage.setItem("user", JSON.stringify(data))
    }

    const logout = () => {
        setUser(null)
        localStorage.removeItem("token")
        localStorage.removeItem("user")
    }

    return (
        <AuthContext.Provider value={{
            user,
            login,
            logout,
            isAuthenticated: !!user
        }}>
            {children}
        </AuthContext.Provider>
    )
}

export function useAuth(){
    const context = useContext(AuthContext)
    if(!context) throw new Error("useAuth must be used within AuthProvider")
    return context
}
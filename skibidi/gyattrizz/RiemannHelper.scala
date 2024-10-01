package gyattrizz.utils

import scala.math.pow

object RiemannHelper {

  // Riemann Zeta function approximation using complex numbers
  def riemannZeta(sReal: Double, sImag: Double): ComplexNumber = {
    var sumReal, sumImag = 0.0
    val limit = 10000 // Large sum for approximation
    
    for (n <- 1 to limit) {
      val nDouble = n.toDouble
      sumReal += pow(nDouble, -sReal) * cos(sImag * math.log(nDouble))
      sumImag += pow(nDouble, -sReal) * sin(sImag * math.log(nDouble))
    }

    ComplexNumber(sumReal, sumImag)
  }
}

package gyattrizz

import gyattrizz.utils.{FourierHelper, InfinitesimalHelper, RiemannHelper}
import scala.math.{Pi, E, cos, sin, pow}

object GyattRizzMain {

  def gyattRizzApply(zetaR: (Double, Double) => ComplexNumber, rInf: Double => ComplexNumber): Double = {
    val fourierTransformResult = FourierHelper.fourierTransform(
      t => ComplexNumber(sin(t), cos(t)),
      limit = 1000, tolerance = InfinitesimalHelper.infinitesimalShift
    )
    
    val riemannZetaComponent = zetaR(fourierTransformResult.real, fourierTransformResult.imag)
    
    val infinitesimalCorrection = rInf(fourierTransformResult.real)
    
    val sigmaRizz = RizzSigma(fourierTransformResult, riemannZetaComponent, infinitesimalCorrection)
    
    sigmaRizz.finalizeTheorem()
  }

  // Entry Point
  def main(args: Array[String]): Unit = {
    val result = gyattRizzApply(RiemannHelper.riemannZeta, InfinitesimalHelper.infCorrection)
    println(s"Gyatt-Rizz Result: $result")
  }
}
